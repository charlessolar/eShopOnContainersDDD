import * as React from 'react';
import Router from 'universal-router';
import Debug from 'debug';

import asyncView from './components/asyncView';
import { StoreType } from './stores';
import { Modules } from './modules';

const debug = new Debug('router');

export function createRouter(store: StoreType, modules: Modules) {

  function isAuthenticated(routerContext: UniversalRouterContext) {
    const { path } = routerContext;
    debug('isAuthenticated', path);
    if (!store.authenticated) {
      setTimeout(() => store.history.push(`/login?nextPath=${path}`), 1);
      throw new Error('redirect');
    }
  }

  function isSetup(routerContext: UniversalRouterContext) {
    const { pathname } = routerContext;
    debug('isSetup', store.status.isSetup);
    if (!store.status.isSetup && !pathname.startsWith('/seed')) {
      setTimeout(() => store.history.push('/seed'), 1);
      throw new Error('redirect');
    }
  }

  const routes: UniversalRouterRoute = {
    path: '',
    action: async (routerContext) => {
      isSetup(routerContext);
    },
    children: [
      {
        path: '',
      },
      // ...modules.auth.routes,
      ...modules.catalog.routes,
      ...modules.configuration.routes
    ]
  };

  return new Router(routes, {
    resolveRoute(routerContext: UniversalRouterContext, params: any) {
      const { route } = routerContext;

      if (typeof route.action === 'function') {
        route.action(routerContext, params);
      }

      if (route.path !== '' && !route.path.startsWith('/login')) {
        // isAuthenticated(routerContext);
      }

      if (typeof route.component === 'function') {
        return route.component(routerContext);
      }
    }
  });
}
