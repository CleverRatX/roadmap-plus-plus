import createClient from 'openapi-fetch'
import { describe, expect, it, vi } from 'vitest'
import { HttpError } from './http-error'
import type { ProblemDetails } from './problem'
import { unwrap } from './unwrap'

type Item = { id: string; title: string }

type TestPaths = {
  '/api/items/{id}': {
    parameters: { query?: never; header?: never; path: { id: string }; cookie?: never }
    get: {
      parameters: { query?: never; header?: never; path: { id: string }; cookie?: never }
      requestBody?: never
      responses: {
        200: { headers: { [name: string]: unknown }; content: { 'application/json': Item } }
        404: { headers: { [name: string]: unknown }; content: { 'application/problem+json': ProblemDetails } }
      }
    }
    put?: never
    post?: never
    delete?: never
    options?: never
    head?: never
    patch?: never
    trace?: never
  }
}

function clientReturning(response: Response) {
  const fetch = vi.fn(async (_request: Request) => response)
  return { fetch, client: createClient<TestPaths>({ baseUrl: 'http://localhost', fetch }) }
}

function problem(status: number, body: ProblemDetails) {
  return new Response(JSON.stringify(body), { status, headers: { 'Content-Type': 'application/problem+json' } })
}

describe('unwrap', () => {
  it('returns typed data and builds the path from parameters', async () => {
    const { fetch, client } = clientReturning(Response.json({ id: '42', title: 'Хакатон' }))

    const item = await unwrap(client.GET('/api/items/{id}', { params: { path: { id: '42' } } }))

    expect(item).toEqual({ id: '42', title: 'Хакатон' })
    expect(fetch.mock.calls[0][0].url).toBe('http://localhost/api/items/42')
  })

  it('throws HttpError with the problem details of the backend', async () => {
    const { client } = clientReturning(
      problem(404, { status: 404, detail: 'Узел не найден.', code: 'planning.item_not_found', requestId: 'req-1' }),
    )

    const error = await unwrap(client.GET('/api/items/{id}', { params: { path: { id: '42' } } })).catch((e: unknown) => e)

    expect(error).toBeInstanceOf(HttpError)
    expect(error).toMatchObject({ status: 404, message: 'Узел не найден.', code: 'planning.item_not_found', requestId: 'req-1' })
  })

  it('exposes validation errors by field', async () => {
    const { client } = clientReturning(problem(400, { status: 400, errors: { title: ['Название обязательно.'] } }))

    const error = await unwrap(client.GET('/api/items/{id}', { params: { path: { id: '42' } } })).catch((e: unknown) => e)

    expect((error as HttpError).fieldErrors).toEqual({ title: ['Название обязательно.'] })
  })

  it('handles a response that is not problem details', async () => {
    const { client } = clientReturning(new Response('Bad Gateway', { status: 502 }))

    const error = await unwrap(client.GET('/api/items/{id}', { params: { path: { id: '42' } } })).catch((e: unknown) => e)

    expect(error).toMatchObject({ status: 502, message: 'Bad Gateway', code: undefined })
  })
})
