import * as React from 'react';
import { action } from 'mobx';
import { observer } from 'mobx-react';

import { Theme, withStyles, WithStyles } from 'material-ui/styles';
import red from 'material-ui/colors/red';
import blue from 'material-ui/colors/blue';
import amber from 'material-ui/colors/amber';

import { SnackbarContent } from 'material-ui/Snackbar';

interface AlertProps {
  type: 'info' | 'warn' | 'error';
  message: string;
  action?: React.ReactElement<any>;
}
interface AlertState {
}

const style = (theme: Theme) => ({
  info: {
    backgroundColor: blue[500]
  },
  warn: {
    backgroundColor: amber[500]
  },
  error: {
    backgroundColor: red[500]
  }
});

class AlertControl extends React.Component<AlertProps & WithStyles<'info' | 'warn' | 'error'>, AlertState> {

  public render() {
    const { message, type, classes, action } = this.props;

    return (
      <SnackbarContent
        classes={{root: classes[type]}}
        message={<span id='message-id'>{message}</span>}
        action={action}
      />
    );
  }
}
export default withStyles(style)(AlertControl);
