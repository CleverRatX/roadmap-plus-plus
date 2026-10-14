import { NavLink } from 'react-router'
import { paths } from '@/shared/config'
import { cx } from '@/shared/lib'
import { ApiStatus } from './ApiStatus'
import styles from './AppHeader.module.css'

const links = [
  { to: paths.roadmap, label: 'Роадмап' },
  { to: paths.today, label: 'Сегодня' },
  { to: paths.knowledge, label: 'База знаний' },
  { to: paths.settings, label: 'Настройки' },
]

export function AppHeader() {
  return (
    <header className={styles.header}>
      <span className={styles.brand}>Roadmap++</span>
      <nav className={styles.nav} aria-label="Разделы">
        {links.map((link) => (
          <NavLink
            key={link.to}
            to={link.to}
            end={link.to === paths.roadmap}
            className={({ isActive }) => cx(styles.link, isActive && styles.active)}
          >
            {link.label}
          </NavLink>
        ))}
      </nav>
      <ApiStatus />
    </header>
  )
}
