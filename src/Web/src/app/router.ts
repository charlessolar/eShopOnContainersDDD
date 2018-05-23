import Debug from 'debug';
import Router from 'universal-router';
import { Modules } from './modules';
import { StoreType } from './stores';

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
      setTimeout(() => store.history.push('/seed'), 100);
      throw new Error('redirect');
    }
  }

  const routes: UniversalRouterRoute = {
    path: '',
    action: (routerContext) => {
      const { pathname } = routerContext;
      debug('isSetup', store.status.isSetup);
      if (!store.status.isSetup && !pathname.startsWith('/seed')) {
        return { redirect: '/seed' };
      }
    },
    children: [
      ...modules.auth.routes,
      ...modules.catalog.routes,
      ...modules.configuration.routes,
      ...modules.administrate.routes,
      ...modules.basket.routes,
      ...modules.orders.routes
    ]
  };

  return new Router(routes, {
    resolveRoute(routerContext: UniversalRouterContext, params: any) {
      const { route } = routerContext;

      if (typeof route.action === 'function') {
        const result = route.action(routerContext, params);
        if (result) {
          return result;
        }
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
