import { fireEvent, render, screen } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { describe, expect, it, vi } from 'vitest'
import { ContextMenu } from './ContextMenu'

describe('ContextMenu', () => {
  it('opens on right click and runs the selected item', async () => {
    const onOpen = vi.fn()
    const onDelete = vi.fn()

    render(
      <ContextMenu
        items={[
          { label: 'Открыть', onSelect: onOpen },
          'separator',
          { label: 'Удалить', onSelect: onDelete, danger: true },
        ]}
      >
        <div>Хакатон</div>
      </ContextMenu>,
    )

    expect(screen.queryByRole('menu')).not.toBeInTheDocument()

    fireEvent.contextMenu(screen.getByText('Хакатон'))
    expect(screen.getByRole('menu')).toBeInTheDocument()

    await userEvent.click(screen.getByRole('menuitem', { name: 'Открыть' }))

    expect(onOpen).toHaveBeenCalledOnce()
    expect(onDelete).not.toHaveBeenCalled()
    expect(screen.queryByRole('menu')).not.toBeInTheDocument()
  })
})
