import { HttpError } from './http-error'
import { toProblem } from './problem'

type FetchResult<T> = {
  data?: T
  error?: unknown
  response: Response
}

export async function unwrap<T>(request: Promise<FetchResult<T>>): Promise<T> {
  const { data, error, response } = await request

  if (!response.ok) {
    throw new HttpError(response.status, toProblem(error))
  }

  return data as T
}
