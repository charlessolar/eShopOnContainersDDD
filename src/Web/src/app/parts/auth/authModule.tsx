import * as React from 'react';
import AsyncView from '../../components/asyncView';
import { StoreType } from '../../stores';
import { LoginStore } from './stores/login';

export class AuthModule {
  public routes: UniversalRouterRoute[];

  constructor(root: StoreType) {

    this.routes = [
      {
        path: '/login',
        component: () => ({
          title: 'Login',
          component: (
            <AsyncView
              actionStore={(store: StoreType) => LoginStore.create({}, { store })}
              getComponent={() => import('./views/login')}
            />
          )
        })
      },
    {
      path: '/logout',
      action: () => {
        root.auth.reset();
        root.history.push('/');
      }
    }];
  }
}
