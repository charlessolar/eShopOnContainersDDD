
import * as ReactDOM from 'react-dom';
import { JsonServiceClient } from '@servicestack/client';
import createBrowserHistory from 'history/createBrowserHistory';
import { onSnapshot } from 'mobx-state-tree';
import Debug from 'debug';

import theme from './theme';

import { config } from './config';
import { Client } from './client';
import { Store, StoreType } from './stores';
import { createModules, Modules } from './modules';

const debug = new Debug('app');

export class App {
  private _store: StoreType;
  private _modules: Modules;

  constructor() {
    const jsonClient = new JsonServiceClient(config.apiUrl);
    const history = createBrowserHistory();

    this._store = Store.create({}, {
      client: jsonClient,
      history,
      theme: theme()
    }) as StoreType;

    this._modules = createModules(this._store as StoreType);
  }

  public async preAuth() {
    await this._store.load();
  }

  public render() {
    const client = new Client(this._store as StoreType, this._modules);
  }

  public async start() {
    debug('start');
    await Promise.all([
      this.preAuth(),
      new Promise((resolve) => {
        setTimeout(() => resolve(), 1000);
      })
    ]);
  }
}
