import type { RouteObject } from 'react-router'
import { AgendaPage } from '@/pages/agenda'
import { KnowledgePage } from '@/pages/knowledge'
import { NotFoundPage } from '@/pages/not-found'
import { RoadmapPage } from '@/pages/roadmap'
import { SettingsPage } from '@/pages/settings'
import { paths } from '@/shared/config'
import { AppLayout } from '../layout/AppLayout'
import { RouteError } from './RouteError'

export const routes: RouteObject[] = [
  {
    path: paths.roadmap,
    Component: AppLayout,
    ErrorBoundary: RouteError,
    children: [
      { index: true, Component: RoadmapPage },
      { path: paths.today, Component: AgendaPage },
      { path: paths.knowledge, Component: KnowledgePage },
      { path: paths.settings, Component: SettingsPage },
      { path: '*', Component: NotFoundPage },
    ],
  },
]
