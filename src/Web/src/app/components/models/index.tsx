import { IModelType } from 'mobx-state-tree';
import * as React from 'react';

export { Using } from './using';
export { Formatted } from './formatted';
export { Field } from './field';
export { Submit } from './submit';
export { Action } from './action';

export interface FormatDefinition {
  normalize?: number;
  precision?: number;
  financial?: boolean;
  currency?: boolean;
  percent?: boolean;
  negative?: boolean;
  commas?: boolean;
  trim?: boolean;
  hideZero?: boolean;
  defaultValue?: string;
}
export interface FieldDefinition {
  input: 'text' | 'textarea' | 'number' | 'selecter' | 'password' | 'dropdown' | 'checkbox' | 'image' | 'daterange';
  label: string;
  required?: boolean;
  type?: string;
  autoComplete?: string;
  allowEmpty?: boolean;
  disabled?: boolean;
  options?: { value: string, label: string }[];

  normalize?: number;

  imageRatio?: number;

  selectStore?: IModelType<any, {
    entries: Map<string, any>,
    loading: boolean,

    list: (term: string, id?: string) => Promise<{}>;
    clear: () => void;
    add: (model: any) => void;
    readonly projection: { id: string, label: string }[];
  }>;
  addComponent?: React.ComponentType<{ onChange: (newVal: any) => void }>;
}
