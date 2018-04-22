import * as React from 'react';
import { parse } from 'qs';
import { observable, action } from 'mobx';

import asyncView from '../../components/asyncView';
import { Context } from '../../context';

import LoginView from './views/login';
import { LoginStore } from './stores/login';

import MeView from './views/me';
import { MeStore } from './stores/me';

export class AuthStore {
  @observable
  public authenticated: boolean;

  @observable
  public token: string;

  public setAuthenticated() {
    this.authenticated = true;
  }
  public setToken(token: string) {
    this.authenticated = true;
    this.token = token;
    localStorage.setItem('JWT', token);
  }

  public getToken() {
    return this.token;
  }
  public reset() {
    this.authenticated = false;
    this.token = '';
  }
}

class Stores {
  public auth: AuthStore;
  public me: MeStore;
  public login: LoginStore;

  constructor(private _context: Context) {
    this.auth = new AuthStore();
    this.me = new MeStore(_context, this.auth);
    this.login = new LoginStore(_context, this.auth);
  }
}

export class AuthModule {
  public stores: Stores;
  public routes: UniversalRouterRoute[];

  constructor(private _context: Context) {
    this.stores = new Stores(_context);

    const AsyncView = asyncView(_context);
    this.routes = [
      {
        path: '/login',
        component: () => ({
          title: 'Login',
          component: (
            <AsyncView
              store={this.stores.login}
              getComponent={() => import('./views/login')}
            />
          )
        })
      },
      {
        path: '/profile',
        component: () => ({
          title: 'Profile',
          component: (
            <AsyncView
              store={this.stores.me}
              getComponent={() => import('./views/me')}
            />
          )
        }),
        action: async () => {
          await this.stores.me.fetch();
        }
      },
    {
      path: '/logout',
      action: () => {
        this.stores.auth.reset();
        localStorage.removeItem('JWT');
        this._context.history.push('/');
      }
    }];
  }
}
