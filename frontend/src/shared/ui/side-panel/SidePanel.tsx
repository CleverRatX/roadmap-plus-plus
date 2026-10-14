import { useId, type ReactNode } from 'react'
import { Button } from '../button/Button'
import styles from './SidePanel.module.css'

type SidePanelProps = {
  title: string
  onClose: () => void
  actions?: ReactNode
  children?: ReactNode
}

export function SidePanel({ title, onClose, actions, children }: SidePanelProps) {
  const titleId = useId()

  return (
    <aside className={styles.panel} aria-labelledby={titleId}>
      <header className={styles.header}>
        <h2 id={titleId} className={styles.title}>
          {title}
        </h2>
        {actions}
        <Button variant="ghost" aria-label="Закрыть панель" onClick={onClose}>
          ×
        </Button>
      </header>
      <div className={styles.body}>{children}</div>
    </aside>
  )
}
