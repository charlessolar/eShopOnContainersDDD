import { parse } from 'qs';
import { observable, action, computed, runInAction } from 'mobx';
import * as validate from 'validate.js';
import Debug from 'debug';

import { Store } from '../../../utils/store';
import { OpStore } from '../../../utils/asyncOp';
import rules from '../validation';
import { Context } from '../../../context';
import { AuthStore } from '../authModule';

import { UserResponse } from '../dtos/login';
import { ProfileRequest, ProfileResponse } from '../dtos/profile';

const debug = new Debug('me');

export class MeStore extends Store {

  @observable
  public email: string;
  @observable
  public firstName: string;
  @observable
  public lastName: string;

  @observable
  public phone: string;

  @observable
  public admin: boolean;

  private _op: OpStore<{}, UserResponse>;
  private _updateOp: OpStore<ProfileRequest, ProfileResponse>;

  constructor(private _context: Context, private _store: AuthStore) {
    super();
    this._op = new OpStore<{}, UserResponse>(_context, payload => {
      return _context.rest.get('identity/user.json').then(r => r.user);
    });
    this._updateOp = new OpStore<ProfileRequest, ProfileResponse>(_context, payload => {
      return _context.rest.put('identity/user.json', { user: payload }).then(r => r.user);
    });
  }

  private redirect() {
    const nextPath = parse(window.location.search.slice(1)).nextPath || '/';
    this._context.history.push(nextPath);
  }

  @computed
  public get updatePayload() {
    const payload: ProfileRequest = {
      first_name: this.firstName,
      last_name: this.lastName,
      phone: this.phone
    };

    return payload;
  }

  @computed
  public get validation() {
    const constraints = {
      first_name: rules.first_name,
      last_name: rules.last_name
    };
    const errors = validate(this.updatePayload, constraints);

    return errors ? { ...errors, errors: true } : { errors: false };
  }

  @action
  public async fetch() {
    try {
      const response = await this._op.fetch();

      runInAction('update data', () => {
        this.email = response.email || '';
        this.firstName = response.first_name || '';
        this.lastName = response.last_name || '';
        this.phone = response.phone || '';
        this.admin = response.admin || false;
      });

      debug('user authenticated: %s', this.email.trim());
      this._store.setAuthenticated();
      const pathname = window.location.pathname;
      if (pathname === '/login') {
        this.redirect();
      }
    } catch (errors) {
      debug('user not logged in');
      localStorage.removeItem('JWT');
    }

  }

  @action
  public async update() {
    this.error = null;

    const errors = this.validation;
    if (errors.errors) {
      debug('validation error:', errors);
      return;
    }

    const payload = this.updatePayload;
    debug('attempting profile update');

    try {
      const response = await this._updateOp.fetch(payload);

      runInAction('update profile', () => {
        this.firstName = response.first_name;
        this.lastName = response.last_name;
        this.phone = response.phone;

        this._context.alertStack.add('info', 'Updated profile!');
      });

    } catch (error) {
      debug('update unsuccessful', error);
      this.error = error;
      this._context.alertStack.add('error', 'Failed to update profile');
    }
  }

}
