
import { JsonServiceClient } from '@servicestack/client';
import { inject } from 'inversify';

import { provideSingleton } from './ioc';
import { DTOs } from './eShop.dtos';

@provideSingleton(Client)
export class Client {
  @inject(JsonServiceClient)
  private _client: JsonServiceClient;

  public query<T>(request: DTOs.IReturn<DTOs.Query<T>>) {
    return this._client.get(request);
  }
  public paged<T>(request: DTOs.IReturn<DTOs.Paged<T>>) {
    return this._client.get(request);
  }
  public command<T>(request: DTOs.IReturnVoid) {
    return this._client.post(request);
  }
}
