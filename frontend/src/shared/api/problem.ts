export type ProblemDetails = {
  type?: string
  title?: string
  status?: number
  detail?: string
  code?: string
  requestId?: string
  errors?: Record<string, string[]>
}

export function toProblem(body: unknown): ProblemDetails {
  if (typeof body === 'object' && body !== null) {
    return body as ProblemDetails
  }

  if (typeof body === 'string' && body.length > 0) {
    return { detail: body }
  }

  return {}
}
