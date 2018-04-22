import { parse } from 'qs';
import { observable, action, computed } from 'mobx';
import * as validate from 'validate.js';
import Debug from 'debug';

import { Store } from '../../../utils/store';
import { OpStore } from '../../../utils/asyncOp';
import { AuthStore } from '../authModule';
import { Context } from '../../../context';
import rules from '../validation';

import { LoginRequest, LoginResponse } from '../dtos/login';

const debug = new Debug('login');

export class LoginStore extends Store {

  @observable
  public email: string;
  @observable
  public password: string;

  private _op: OpStore<LoginRequest, LoginResponse>;

  constructor(private _context: Context, private _store: AuthStore) {
    super();
    this._op = new OpStore<LoginRequest, LoginResponse>(_context, payload => {
      return _context.rest.post('identity/tokens.json', { token_form: payload });
    });
  }

  private redirect() {
    const nextPath = parse(window.location.search.slice(1)).nextPath || '/';
    this._context.history.push(nextPath);
  }

  @computed
  public get loginPayload() {
    const payload: LoginRequest = {
      email: (this.email || '').trim(),
      password: (this.password || '').trim()
    };

    return payload;
  }

  @computed
  public get validation() {
    const constraints = {
      email: rules.email
    };
    const errors = validate(this.loginPayload, constraints);

    return errors ? { ...errors, errors: true } : { errors: false };
  }

  @action
  public fetch() {
    return new Promise<void>(resolve => resolve());
  }

  @action
  public async login() {
    this.error = null;

    const errors = this.validation;
    if (errors.errors) {
      debug('validation error:', errors);
      return;
    }

    const payload = this.loginPayload;
    debug('attempting login: %s', payload.email.trim());

    try {
      const response = await this._op.fetch(payload);
      const { token } = response;
      this._store.setToken(token.key);
      await this._context.parts.auth.stores.me.fetch();

      debug('login successful: %s', token.key);
      this.redirect();
    } catch (error) {
      debug('login unsuccessful', error);
      this.error = error;
      this._context.alertStack.add('error', 'Failed to login');
      // console.error('login ', errors);
      localStorage.removeItem('JWT');
    }
  }
}
