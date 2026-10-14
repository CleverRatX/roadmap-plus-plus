import type { ReactNode } from 'react'
import { Dialog as RadixDialog } from 'radix-ui'
import styles from './Dialog.module.css'

type DialogProps = {
  title: string
  description?: string
  trigger?: ReactNode
  open?: boolean
  onOpenChange?: (open: boolean) => void
  footer?: ReactNode
  children?: ReactNode
}

export function Dialog({ title, description, trigger, open, onOpenChange, footer, children }: DialogProps) {
  return (
    <RadixDialog.Root open={open} onOpenChange={onOpenChange}>
      {trigger && <RadixDialog.Trigger asChild>{trigger}</RadixDialog.Trigger>}
      <RadixDialog.Portal>
        <RadixDialog.Overlay className={styles.overlay} />
        <RadixDialog.Content className={styles.content} {...(description ? {} : { 'aria-describedby': undefined })}>
          <RadixDialog.Title className={styles.title}>{title}</RadixDialog.Title>
          {description && <RadixDialog.Description className={styles.description}>{description}</RadixDialog.Description>}
          {children && <div className={styles.body}>{children}</div>}
          {footer && <div className={styles.footer}>{footer}</div>}
          <RadixDialog.Close className={styles.close} aria-label="Закрыть">
            ×
          </RadixDialog.Close>
        </RadixDialog.Content>
      </RadixDialog.Portal>
    </RadixDialog.Root>
  )
}

export const DialogClose = RadixDialog.Close
