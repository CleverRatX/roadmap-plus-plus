import { QueryClient } from '@tanstack/react-query'
import { HttpError } from '@/shared/api'

export function shouldRetry(failureCount: number, error: Error): boolean {
  const isClientError = error instanceof HttpError && error.status < 500
  return !isClientError && failureCount < 2
}

export function createQueryClient(): QueryClient {
  return new QueryClient({
    defaultOptions: {
      queries: {
        staleTime: 30_000,
        retry: shouldRetry,
      },
    },
  })
}
