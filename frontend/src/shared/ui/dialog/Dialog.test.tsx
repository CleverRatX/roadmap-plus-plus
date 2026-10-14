import { render, screen } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { describe, expect, it } from 'vitest'
import { Button } from '../button/Button'
import { Dialog } from './Dialog'

describe('Dialog', () => {
  it('opens from the trigger and closes with the close button', async () => {
    render(
      <Dialog title="Новое событие" trigger={<Button>Создать</Button>}>
        <p>Содержимое</p>
      </Dialog>,
    )

    expect(screen.queryByRole('dialog')).not.toBeInTheDocument()

    await userEvent.click(screen.getByRole('button', { name: 'Создать' }))
    expect(screen.getByRole('dialog', { name: 'Новое событие' })).toBeInTheDocument()

    await userEvent.click(screen.getByRole('button', { name: 'Закрыть' }))
    expect(screen.queryByRole('dialog')).not.toBeInTheDocument()
  })

  it('closes on Escape', async () => {
    render(<Dialog title="Новое событие" trigger={<Button>Создать</Button>} />)

    await userEvent.click(screen.getByRole('button', { name: 'Создать' }))
    await userEvent.keyboard('{Escape}')

    expect(screen.queryByRole('dialog')).not.toBeInTheDocument()
  })
})
