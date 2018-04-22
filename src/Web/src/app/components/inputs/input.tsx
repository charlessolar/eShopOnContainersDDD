import * as React from 'react';
import { observable, action, computed } from 'mobx';
import { observer } from 'mobx-react';

import { withStyles, WithStyles } from 'material-ui/styles';
import Input, { InputLabel } from 'material-ui/Input';
import { FormControl, FormHelperText } from 'material-ui/Form';

import { Context } from '../../context';

interface InputProps {
  id: string;
  required?: boolean;
  label: string;
  error: string;
  type?: string;
  value?: any;
  autoComplete?: string;
  onChange: (newVal: string) => void;
}

class InputControl extends React.Component<InputProps & WithStyles<'formControl'>, {}> {

  @action
  private handleChange(e) {
    this.props.onChange(e.target.value);
  }

  public render() {
    const { id, label, required, error, type, value, autoComplete, classes } = this.props;

    return (
      <FormControl required={required} className={classes.formControl} error={error ? true : false} aria-describedby={id + '-text'}>
        <InputLabel htmlFor={id}>{label}</InputLabel>
        <Input id={id} onChange={e => this.handleChange(e)} type={type || 'text'} autoComplete={autoComplete} value={value}/>
        <FormHelperText id={id + '-text'}>{error}</FormHelperText>
      </FormControl>
    );
  }
}

export default function(context: Context) {

  const styles = theme => ({
    formControl: {
      marginLeft: theme.spacing.unit,
      marginRight: theme.spacing.unit,
      width: 400,
    },
  });

  return withStyles(styles)(observer(InputControl));
}
