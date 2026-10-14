import { Link } from 'react-router'
import { paths } from '@/shared/config'
import { Page } from '@/shared/ui'

export function NotFoundPage() {
  return (
    <Page title="Страница не найдена">
      <p>
        Такой страницы нет. <Link to={paths.roadmap}>Вернуться к роадмапу</Link>
      </p>
    </Page>
  )
}
