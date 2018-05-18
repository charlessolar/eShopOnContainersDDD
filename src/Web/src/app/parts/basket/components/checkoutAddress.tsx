import * as React from 'react';
import { observer } from 'mobx-react';

import { Theme, withStyles, WithStyles } from 'material-ui/styles';
import Typography from 'material-ui/Typography';
import Grid from 'material-ui/Grid';
import Button from 'material-ui/Button';

import { inject } from '../../../utils';
import { Using, Formatted, Field } from '../../../components/models';

import { AddressStoreType, AddressStoreModel } from '../stores/address';
import { CheckoutStoreType } from '../stores/checkout';

interface AddressProps {
  handleNext: () => void;
  handlePrev: () => void;

  store?: AddressStoreType;
  checkout: CheckoutStoreType;
}

const styles = (theme: Theme) => ({

});

@inject(AddressStoreModel)
@observer
class AddressView extends React.Component<AddressProps & WithStyles<never>, {}> {

  public render() {
    const { handleNext, handlePrev } = this.props;

    return (
      <div>
        <Button variant='raised' onClick={handleNext}>Next</Button>
      </div>
    );
  }
}

export default withStyles(styles)<AddressProps>(AddressView);
