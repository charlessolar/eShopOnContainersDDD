import { IModelType, types, flow } from 'mobx-state-tree';
import { Component } from 'react';

export interface ComponentDefinition {
  input: string;
  label: string;
  required?: boolean;
}

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

export interface FormType<T> {
  loading: boolean;
  readonly valid: boolean;
  readonly form: { [idx: string]: ComponentDefinition };
  readonly validation: { [idx: string]: string };
  changeValue(name: string, newVal: any): void;
  submit(): void;
  payload: T;
}
export function Form<S, T extends { readonly form: { [idx: string]: ComponentDefinition }, readonly valid: boolean, readonly validation: { [idx: string]: string }, submit(): void }>(subtype: IModelType<S, T>, env: any, sideEffect?: (model: T, success: boolean) => void): FormType<T> {
  return types.model(
    {
      loading: types.optional(types.boolean, false),
      payload: types.optional(subtype, {})
    })
    .views(self => ({
      get valid() {
        return self.payload.valid;
      },
      get form() {
        return self.payload.form;
      },
      get validation() {
        return self.payload.validation;
      }
    }))
    .actions(self => {
      const changeValue = (name: string, newVal: any) => {
        self.payload[name] = newVal;
      };
      const submit = flow(function*() {
        self.loading = true;
        let success = false;
        try {
          yield self.payload.submit();
          success = true;
        } catch (e) {
          console.log('submit error:', e);
        }
        if (sideEffect) {
          sideEffect(self.payload, success);
        }
        self.loading = false;
      });
      return { changeValue, submit };
    })
    .create({}, env);
}
