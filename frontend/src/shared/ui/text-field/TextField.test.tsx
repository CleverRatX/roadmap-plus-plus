import { render, screen } from '@testing-library/react'
import { describe, expect, it } from 'vitest'
import { TextField } from './TextField'

describe('TextField', () => {
  it('is labelled by its label', () => {
    render(<TextField label="Название" />)

    expect(screen.getByLabelText('Название')).toBeInTheDocument()
  })

  it('marks the input invalid and describes it with the error', () => {
    render(<TextField label="Название" error="Название обязательно." />)

    const input = screen.getByLabelText('Название')
    expect(input).toHaveAttribute('aria-invalid', 'true')
    expect(input).toHaveAccessibleDescription('Название обязательно.')
  })
})
