import { useQuery } from '@tanstack/react-query'
import { fetchHealth } from '@/shared/api'
import styles from './AppHeader.module.css'

type Status = 'checking' | 'ok' | 'down'

const labels: Record<Status, string> = {
  checking: 'Проверяем API…',
  ok: 'API работает',
  down: 'API недоступен',
}

export function ApiStatus() {
  const health = useQuery({
    queryKey: ['health'],
    queryFn: ({ signal }) => fetchHealth(signal),
    refetchInterval: 30_000,
    retry: false,
  })

  const status: Status = health.isPending ? 'checking' : health.data === 'Healthy' ? 'ok' : 'down'

  return (
    <span className={styles.status} data-status={status} role="status">
      {labels[status]}
    </span>
  )
}
