import { createBrowserRouter, RouterProvider } from 'react-router'
import { QueryProvider } from './providers/QueryProvider'
import { routes } from './router/routes'

const router = createBrowserRouter(routes)

export function App() {
  return (
    <QueryProvider>
      <RouterProvider router={router} />
    </QueryProvider>
  )
}
