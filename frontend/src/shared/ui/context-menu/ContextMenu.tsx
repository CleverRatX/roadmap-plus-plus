import type { ReactElement } from 'react'
import { ContextMenu as RadixContextMenu } from 'radix-ui'
import { cx } from '@/shared/lib'
import styles from './ContextMenu.module.css'

export type ContextMenuItem =
  | {
      label: string
      onSelect: () => void
      danger?: boolean
      disabled?: boolean
    }
  | 'separator'

type ContextMenuProps = {
  items: ContextMenuItem[]
  children: ReactElement
}

export function ContextMenu({ items, children }: ContextMenuProps) {
  return (
    <RadixContextMenu.Root>
      <RadixContextMenu.Trigger asChild>{children}</RadixContextMenu.Trigger>
      <RadixContextMenu.Portal>
        <RadixContextMenu.Content className={styles.content}>
          {items.map((item, index) =>
            item === 'separator' ? (
              <RadixContextMenu.Separator key={`separator-${index}`} className={styles.separator} />
            ) : (
              <RadixContextMenu.Item
                key={item.label}
                className={cx(styles.item, item.danger && styles.danger)}
                disabled={item.disabled}
                onSelect={item.onSelect}
              >
                {item.label}
              </RadixContextMenu.Item>
            ),
          )}
        </RadixContextMenu.Content>
      </RadixContextMenu.Portal>
    </RadixContextMenu.Root>
  )
}
