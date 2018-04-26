import * as React from 'react';
import { observer } from 'mobx-react';

import { Theme, withStyles, WithStyles } from 'material-ui/styles';
import Button from 'material-ui/Button';

import { inject, models, FormType } from '../../../utils';
import { Form, Field } from '../../../components/forms';

import { StoreType } from '../../../stores';
import { Brand, BrandFormType, BrandsType } from '../models/brands';

interface BrandsProps {
  store: BrandsType;
}

@inject((store) => store.catalog.Brands)
@observer
export default class Brands extends React.Component<BrandsProps, {}> {
  private _brand: FormType<BrandFormType>;

  constructor(props) {
    super(props);
    const { store } = this.props;

    this._brand = store.brandForm();
  }

  public handleChange(newVal: string) {
    const { store } = this.props;

  }
  public handleSubmit() {
    const { store } = this.props;

    if (!this._brand.valid) {
      return;
    }

    store.List.add(this._brand.payload);
  }

  public render() {
    const { store } = this.props;

    return (
      <div>
        <h3>Hello world</h3>
        <ul>
        {Array.from(store.List.entries.values(), (brand, key) => (
          <li key={key}>{brand.brand} <Button onClick={() => store.List.remove(brand.id)}>Remove</Button></li>
        ))}
        </ul>

        <h4>New Brand</h4>

        <Form form={this._brand}>
          <Field field='brand' />
        </Form>
      </div>
    );

    // <Input property='brand' onChange={}
    // <Text id='brand' required label='Brand:' error={this._brand.payload.validation} onChange={(val) => this.handleChange(val)} />
  }
}
