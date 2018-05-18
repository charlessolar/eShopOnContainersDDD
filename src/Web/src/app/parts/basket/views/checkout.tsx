import * as React from 'react';
import { observer } from 'mobx-react';
import { hot } from 'react-hot-loader';

import { Theme, withStyles, WithStyles } from 'material-ui/styles';

import { Using, Formatted } from '../../../components/models';
import { CheckoutStoreType, CheckoutStoreModel } from '../stores/checkout';

interface CheckoutProps {
  store?: CheckoutStoreType;
}

const styles = (theme: Theme) => ({

});

@observer
class CheckoutView extends React.Component<CheckoutProps & WithStyles<never>, {}> {
  public componentDidMount() {
    const { store } = this.props;

    store.validateBasket();
  }

  public render() {
    const { store, classes } = this.props;

    return (
      <div></div>
    );
  }
}

export default hot(module)(withStyles(styles as any)(CheckoutView));
