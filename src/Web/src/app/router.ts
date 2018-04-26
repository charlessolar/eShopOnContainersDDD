import * as React from 'react';
import Router from 'universal-router';
import Debug from 'debug';

import asyncView from './components/asyncView';
import { StoreType } from './stores';
import { Modules } from './modules';

const debug = new Debug('router');

export function createRouter(store: StoreType, modules: Modules) {

  const routes: UniversalRouterRoute = {
    path: '',
    children: [
      ...modules.catalog.routes
    ]
  };

  return new Router(routes, {
    resolveRoute(routerContext: UniversalRouterContext, params: any) {
      const { route } = routerContext;

      if (typeof route.action === 'function') {
        return route.action(routerContext, params);
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
