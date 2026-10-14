import { Outlet } from 'react-router'
import { AppHeader } from '@/widgets/app-header'
import styles from './AppLayout.module.css'

export function AppLayout() {
  return (
    <div className={styles.shell}>
      <AppHeader />
      <main className={styles.main}>
        <Outlet />
      </main>
    </div>
  )
}
