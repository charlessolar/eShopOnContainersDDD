import * as React from 'react';
import { observer } from 'mobx-react';
import { hot } from 'react-hot-loader';

import { Theme, withStyles, WithStyles } from '@material-ui/core/styles';
import Card from '@material-ui/core/Card';
import CardActions from '@material-ui/core/CardActions';
import CardContent from '@material-ui/core/CardContent';
import CardMedia from '@material-ui/core/CardMedia';
import Paper from '@material-ui/core/Paper';
import Grid from '@material-ui/core/Grid';
import Typography from '@material-ui/core/Typography';
import Button from '@material-ui/core/Button';

import { inject, models } from '../../../utils';
import { Using, Field, Submit } from '../../../components/models';

import { StoreType } from '../../../stores';
import { LoginType, LoginStore } from '../stores/login';

interface LoginProps {
  store?: LoginType;
}

const styles = (theme: Theme) => ({
  container: {
    'text-align': 'center',
    'width': 600,
    'margin': 20,
    'padding': 30,
    'display': 'flex',
  },
  textField: {
    marginLeft: theme.spacing.unit,
    marginRight: theme.spacing.unit,
    width: 400,
  },
  button: {
    margin: theme.spacing.unit,
  },
  menu: {
    width: 200,
  },
});

@observer
class LoginView extends React.Component<LoginProps & WithStyles<'container' | 'button' | 'textField' | 'menu'>, {}> {
  public render() {
    const { store, classes } = this.props;

    return (
      <Paper className={classes.container} elevation={3}>
        <Using model={store}>
          <Grid container spacing={24}>
            <Grid item xs={12}>
              <Field field='username' />
              <Field field='password' />
            </Grid>
            <Grid item xs={12}>
              <Submit text='Sign in' buttonProps={{variant: 'raised', color: 'primary' }} className={classes.button} />
              <Button color='secondary' variant='raised' className={classes.button}>
                Register
              </Button>
            </Grid>
            <Grid item xs={12}>
              <Typography component='aside'>
                Forgot Password
                </Typography>
                <Typography component='title'>
                  UserName: administrator <br/>
                  Password: 12345678
                </Typography>
            </Grid>
          </Grid>
        </Using>
      </Paper>
    );
  }
}

export default hot(module)(withStyles(styles)(LoginView));
