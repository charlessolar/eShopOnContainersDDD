
import * as ReactDOM from 'react-dom';
import Debug from 'debug';

import { Context } from './context';
import { Client } from './client';

const debug = new Debug('app');

export class App {
  public context: Context;

  constructor() {
    this.context = new Context();

    this.context.rest.setJwtSelector(() => this.context.parts.auth.stores.auth.getToken());
  }

  public async preAuth() {
    const token = localStorage.getItem('JWT');
    if (token) {
      // auth set token
      this.context.parts.auth.stores.auth.setToken(token);
    }
    await this.context.parts.auth.stores.me.fetch();
  }

  public render() {
    const client = new Client(this.context);
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
