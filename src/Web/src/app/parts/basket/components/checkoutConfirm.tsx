import * as React from 'react';
import { observer } from 'mobx-react';

import { Theme, withStyles, WithStyles } from '@material-ui/core/styles';
import Typography from '@material-ui/core/Typography';
import Grid from '@material-ui/core/Grid';
import Button from '@material-ui/core/Button';

import { inject } from '../../../utils';
import { Using, Formatted, Field } from '../../../components/models';

import { ConfirmStoreType, ConfirmStoreModel } from '../stores/confirm';
import { CheckoutStoreType } from '../stores/checkout';

interface ConfirmProps {
  handleNext: () => void;
  handlePrev: () => void;

  store?: ConfirmStoreType;
  checkout: CheckoutStoreType;
}

const styles = (theme: Theme) => ({

});

@inject(ConfirmStoreModel)
@observer
class ConfirmView extends React.Component<ConfirmProps & WithStyles<never>, {}> {

  public render() {
    const { handleNext, handlePrev } = this.props;

    return (
      <div>
        <Button variant='raised' onClick={handlePrev}>Prev</Button>
        <Button variant='raised' onClick={handleNext}>Confirm</Button>
      </div>
    );
  }
}

export default withStyles(styles)<ConfirmProps>(ConfirmView);
