import Debug from 'debug';
import Router from 'universal-router';
import { Modules } from './modules';
import { StoreType } from './stores';

const debug = new Debug('router');

export function createRouter(store: StoreType, modules: Modules) {

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

      if (typeof route.component === 'function') {
        return route.component(routerContext);
      }
    }
  });
}
