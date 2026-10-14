import { QueryClient, QueryClientProvider } from '@tanstack/react-query'
import { render, screen } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { createMemoryRouter, RouterProvider } from 'react-router'
import { describe, expect, it, vi } from 'vitest'
import { routes } from './routes'

function renderApp(path: string, health: Response) {
  vi.stubGlobal('fetch', vi.fn(async () => health.clone()))

  const client = new QueryClient({ defaultOptions: { queries: { retry: false } } })
  const router = createMemoryRouter(routes, { initialEntries: [path] })

  render(
    <QueryClientProvider client={client}>
      <RouterProvider router={router} />
    </QueryClientProvider>,
  )
}

describe('routes', () => {
  it('opens the roadmap on the root path', async () => {
    renderApp('/', new Response('Healthy'))

    expect(await screen.findByRole('heading', { level: 1, name: 'Роадмап' })).toBeInTheDocument()
    expect(screen.getByRole('link', { name: 'Роадмап' })).toHaveAttribute('aria-current', 'page')
  })

  it('navigates between sections through the header', async () => {
    renderApp('/', new Response('Healthy'))

    await userEvent.click(await screen.findByRole('link', { name: 'Сегодня' }))

    expect(await screen.findByRole('heading', { level: 1, name: 'Сегодня / завтра' })).toBeInTheDocument()
    expect(screen.getByRole('link', { name: 'Роадмап' })).not.toHaveAttribute('aria-current')
  })

  it('shows not found for an unknown path', async () => {
    renderApp('/no-such-page', new Response('Healthy'))

    expect(await screen.findByRole('heading', { level: 1, name: 'Страница не найдена' })).toBeInTheDocument()
  })

  it('shows that the API works', async () => {
    renderApp('/', new Response('Healthy'))

    expect(await screen.findByText('API работает')).toBeInTheDocument()
  })

  it('shows that the API is down when the database is unhealthy', async () => {
    renderApp('/', new Response('Unhealthy', { status: 503 }))

    expect(await screen.findByText('API недоступен')).toBeInTheDocument()
  })
})
