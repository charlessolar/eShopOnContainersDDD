import * as React from 'react';
import { types, getRoot } from 'mobx-state-tree';

import AsyncView from '../../components/asyncView';
import { models } from '../../utils';
import { StoreType } from '../../stores';

import { LoginType, LoginStore } from './stores/login';

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
