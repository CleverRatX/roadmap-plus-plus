import { describe, expect, it } from 'vitest'
import { HttpError } from '@/shared/api'
import { shouldRetry } from './query-client'

describe('shouldRetry', () => {
  it('does not retry client errors', () => {
    expect(shouldRetry(0, new HttpError(404))).toBe(false)
  })

  it('retries server and network errors twice', () => {
    expect(shouldRetry(0, new HttpError(502))).toBe(true)
    expect(shouldRetry(1, new TypeError('Failed to fetch'))).toBe(true)
    expect(shouldRetry(2, new TypeError('Failed to fetch'))).toBe(false)
  })
})
