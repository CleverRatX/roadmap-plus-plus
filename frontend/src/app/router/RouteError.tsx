import { isRouteErrorResponse, Link, useRouteError } from 'react-router'
import { paths } from '@/shared/config'
import { Page } from '@/shared/ui'

export function RouteError() {
  const error = useRouteError()
  const details = isRouteErrorResponse(error)
    ? `${error.status} ${error.statusText}`
    : error instanceof Error
      ? error.message
      : null

  return (
    <Page title="Что-то сломалось">
      <p>Страницу не удалось показать. Обновите её или вернитесь к роадмапу.</p>
      {details && <pre>{details}</pre>}
      <p>
        <Link to={paths.roadmap}>Вернуться к роадмапу</Link>
      </p>
    </Page>
  )
}
