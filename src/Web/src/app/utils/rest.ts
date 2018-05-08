import Axios from 'axios';
import Qs from 'qs';

import Debug from 'debug';

import { config } from '../config';

const debug = new Debug('rest');

export class Rest {
  private _jwtSelector: () => string;

  constructor(private _options: any = {}) { }

  private ajax(url: string, method: string, data: any = null, params: any = null) {
    debug('ajax url: %s, method: %s, options %s, params: ', url, method, JSON.stringify(this._options), params);

    const headers = {
      'Content-Type': 'application/json; charset=utf-8'
    };

    if (this._jwtSelector) {
      const jwt = this._jwtSelector();
      if (jwt) {
        headers['Authorization'] = `Bearer ${jwt}`;
      }
    }

    return Axios({
      method,
      baseURL: config.apiUrl,
      url,
      params,
      data,
      withCredentials: true,
      headers,
      timeout: 30e3,
      paramsSerializer: (params) => {
        return Qs.stringify(params, { arrayFormat: 'brackets' });
      }
    }).then(res => res.data).catch(error => {
      debug('ajax error: ', error);
      throw error;
    });
  }

  public setJwtSelector(selector: () => string) {
    this._jwtSelector = selector;
  }

  public get(url, params: any = {}) {
    return this.ajax(url, 'GET', null, params);
  }
  public post(url, data: any = {}) {
    return this.ajax(url, 'POST', data);
  }
  public put(url, data: any = {}) {
    return this.ajax(url, 'PUT', data);
  }
  public delete(url, data: any = {}) {
    return this.ajax(url, 'DELETE', data);
  }
}
