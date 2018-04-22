import * as React from 'react';
import Router from 'universal-router';
import Debug from 'debug';

import asyncView from './components/asyncView';
import { Context } from './context';

const debug = new Debug('router');

export function createRouter(context: Context) {
  const AsyncView = asyncView(context);

  function isAuthenticated(routerContext: UniversalRouterContext) {
    const { path } = routerContext;
    debug('isAuthenticated', path);
    const { authenticated } = context.parts.auth.stores.auth;
    if (!authenticated) {
      setTimeout(() => context.history.push(`/login?nextPath=${path}`), 1);
      throw new Error('redirect');
    }
  }
  function isAdmin(routerContext: UniversalRouterContext) {
    const { path } = routerContext;
    debug('isAdmin', path);
    const { authenticated } = context.parts.auth.stores.auth;
    if (!authenticated) {
      setTimeout(() => context.history.push(`/login?nextPath=${path}`), 1);
      throw new Error('redirect');
    }

    if (context.parts.auth.stores.me.admin) {
      setTimeout(() => context.history.push(`/login?nextPath=${path}`), 1);
      throw new Error('redirect');
    }
  }

  function createPart(partName: string, partCreate: any, routerContext: any) {
    const part = partCreate.default(context);
    context.parts[partName] = part;
    routerContext.route.children = part.routes();
    return routerContext.next();
  }

  const routes: UniversalRouterRoute = {
    path: '',
    children: [
      {
        path: '',
        action: async (routerContext) => {
          const { authenticated } = context.parts.auth.stores.auth;
          if (!authenticated) {
            setTimeout(() => context.history.push(`/login`), 1);
            throw new Error('redirect');
          }
          if (context.parts.auth.stores.me.admin) {
            // setTimeout(() => context.history.push(`/admin`), 1);
            setTimeout(() => context.history.push(`/merchant`), 1);
          } else {
            setTimeout(() => context.history.push(`/merchant`), 1);
          }

        }
      },
      ...context.parts.auth.routes,
      ...context.parts.admin.routes,
      ...context.parts.menu.routes,
      ...context.parts.merchant.routes,
      // {
      //   path: '/profile',
      //   action: async (routerContext) => {
      //     isAuthenticated(routerContext);
      //     const partCreate = await import('./parts/profile/profileModule');
      //     return createPart('profile', partCreate, routerContext);
      //   }
      // },
      // {
      //   path: '/users',
      //   action: async (routerContext) => {
      //     isAuthenticated(routerContext);
      //     const partCreate = await import('./parts/admin/adminModule');
      //     return createPart('admin', partCreate, routerContext);
      //   }
      // }
    ]
  };

  return new Router(routes, {
    resolveRoute(routerContext: UniversalRouterContext, params: any) {
      const { route } = routerContext;

      if (typeof route.action === 'function') {
        return route.action(routerContext, params);
      }

      if (route.path !== '' && !route.path.startsWith('/login')) {
        isAuthenticated(routerContext);
      }
      if (route.path.startsWith('/admin')) {
        isAdmin(routerContext);
      }

      if (typeof route.component === 'function') {
        return route.component(routerContext);
      }
    }
  });
}
