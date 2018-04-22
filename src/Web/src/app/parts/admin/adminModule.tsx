import * as React from 'react';
import { parse } from 'qs';
import { observable, action } from 'mobx';

import asyncView from '../../components/asyncView';
import { OpStore } from '../../utils/asyncOp';
import { Context } from '../../context';

class Stores {

  constructor(private _context: Context) {
  }
}

export class AdminModule {
  public stores: Stores;
  public routes: UniversalRouterRoute[];

  constructor(private _context: Context) {
    this.stores = new Stores(_context);

    const AsyncView = asyncView(_context);
    this.routes = [];
  }
}
