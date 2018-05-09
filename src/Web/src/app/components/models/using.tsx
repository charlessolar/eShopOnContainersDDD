import * as React from 'react';
import { observable, action, computed, runInAction } from 'mobx';
import { observer } from 'mobx-react';
import { flow, applyPatch, IStateTreeNode } from 'mobx-state-tree';

import { FormatDefinition, FieldDefinition } from '.';
import { Data } from '../../utils/image';

export const UsingContext = React.createContext<UsingType<any>>(undefined);

interface Payload {
  readonly validation?: { [idx: string]: any };
  readonly form?: { [idx: string]: FieldDefinition };
  readonly formatting?: { [idx: string]: FormatDefinition };
  submit?: () => Promise<{}>;
}

export class UsingType<T extends Payload & IStateTreeNode> {
  public payload: T;
  @observable
  public loading: boolean;

  constructor(model: T) {
    this.payload = model;
    this.loading = false;
  }

  @computed
  public get valid() {
    return this.payload && this.payload.validation === undefined;
  }
  @computed
  public get formatting() {
    return this.payload && this.payload.formatting ? this.payload.formatting : {};
  }
  @computed
  public get form() {
    return this.payload && this.payload.form ? this.payload.form : {};
  }
  @computed
  public get validation() {
    return this.payload && this.payload.validation ? this.payload.validation : {};
  }

  public changeValue(name: string, newVal: string | number | Data | { from: string, to: string} | any) {
    applyPatch(this.payload, { op: 'replace', path: name, value: newVal});
  }
  public pushValue(name: string, val: string | number) {
    const existing = [...Array.from(this.payload[name]), val];

    applyPatch(this.payload, { op: 'replace', path: name, value: existing});
  }
  public removeValue(name: string, val: string | number) {
    const existing = Array.from(this.payload[name]).filter(x => x !== val);

    applyPatch(this.payload, { op: 'replace', path: name, value: existing});
  }

  @action
  public async submit() {
    this.loading = true;
    let success = false;
    try {
      await this.payload.submit();
      success = true;
    } catch (e) {
      // console.log('submit error:', e);
    }
    runInAction(() => {
      this.loading = false;
    });
  }
}

interface UsingProps<T extends Payload> {
  model: T;
}

export class Using<T extends Payload> extends React.Component<UsingProps<T>, {}> {

  public render() {
    const { children, model } = this.props;

    return (
      <UsingContext.Provider value={new UsingType(model)}>
        {children}
      </UsingContext.Provider>
    );
  }
}
