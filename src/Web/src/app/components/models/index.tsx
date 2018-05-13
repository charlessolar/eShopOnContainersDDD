import { types, IModelType, flow, IStateTreeNode } from 'mobx-state-tree';

export { Using } from './using';
export { Formatted } from './formatted';
export { Field } from './field';
export { Submit } from './submit';

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
  options?: { value: string, label: string }[];

  imageRatio?: number;

  projectionStore?: any;
  projection?: (store: any, term: string) => Promise<{ id: string, label: string }[]>;
  select?: (store: any, id: string) => any;
  getIdentity?: (model: any) => string;
}
