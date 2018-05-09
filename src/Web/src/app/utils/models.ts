import { IModelType, types, flow, getSnapshot } from 'mobx-state-tree';
import { Component } from 'react';
import { StoreType } from '../stores';

const ResponseError = types.model(
  'ResponseError',
  {
    errorCode: types.optional(types.string, ''),
    fieldName: types.optional(types.string, ''),
    message: types.optional(types.string, ''),
    meta: types.optional(types.map(types.string), {})
  }
);

const ResponseStatus = types.model(
  'ResponseStatus',
  {
    errorCode: types.optional(types.string, ''),
    message: types.optional(types.string, ''),
    stackTrace: types.optional(types.string, ''),
    errors: types.optional(types.array(ResponseError), [])
  })
  .views(self => ({
    get success() {
      return self.errors.length === 0;
    }
  }));

export function PagedResponse<S, T>(subtype: IModelType<S, T>) {
  return types.model({
    roundTripMs: types.number,
    total: types.number,
    records: types.array(subtype),
    responseStatus: types.optional(ResponseStatus, {})
  });
}

export function QueryResponse<S, T>(subtype: IModelType<S, T>) {
  return types.model({
    roundTripMs: types.number,
    payload: subtype
  });
}
