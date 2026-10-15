import { HttpError } from './http-error'

export type HealthStatus = 'Healthy' | 'Degraded' | 'Unhealthy'

const statuses: readonly string[] = ['Healthy', 'Degraded', 'Unhealthy'] satisfies HealthStatus[]

export async function fetchHealth(signal?: AbortSignal): Promise<HealthStatus> {
  const response = await fetch('/api/health', { signal })
  const text = await response.text()

  if (statuses.includes(text)) {
    return text as HealthStatus
  }

  throw new HttpError(response.status)
}
