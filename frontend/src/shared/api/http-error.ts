import type { ProblemDetails } from './problem'

export class HttpError extends Error {
  readonly status: number
  readonly problem: ProblemDetails

  constructor(status: number, problem: ProblemDetails = {}) {
    super(problem.detail ?? problem.title ?? `Ошибка API: ${status}`)
    this.name = 'HttpError'
    this.status = status
    this.problem = problem
  }

  get code(): string | undefined {
    return this.problem.code
  }

  get requestId(): string | undefined {
    return this.problem.requestId
  }

  get fieldErrors(): Record<string, string[]> {
    return this.problem.errors ?? {}
  }
}
