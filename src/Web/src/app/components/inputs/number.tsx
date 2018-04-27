import * as React from 'react';
import { observable, action, computed } from 'mobx';
import { observer } from 'mobx-react';

import { withStyles, WithStyles } from 'material-ui/styles';
import Input, { InputLabel } from 'material-ui/Input';
import { FormControl, FormHelperText } from 'material-ui/Form';
import NumberFormat from 'react-number-format';
import MaskedInput from 'react-text-mask';
import numeral from 'numeral';

interface NumberProps {
  id: string;
  required?: boolean;
  label: string;
  error?: any;
  type?: string;
  value?: any;
  autoComplete?: string;
  format?: string;
  onChange?: (newVal: number) => void;
  onKeyDown?: (key: number, value: string) => void;
}

@observer
class NumberControl extends React.Component<NumberProps & WithStyles<'formControl'>, {}> {

  @action
  private handleChange(e: React.ChangeEvent<HTMLTextAreaElement | HTMLInputElement>) {
    if (this.props.onChange) {
      const num = numeral(e.target.value);

      this.props.onChange(num.value());
    }
  }
  @action
  private handleKeydown(e: React.KeyboardEvent<HTMLTextAreaElement | HTMLInputElement>) {
    if (this.props.onKeyDown) {
      this.props.onKeyDown(e.which, (e.target as any).value.trim());
    }
  }

  public render() {
    const { id, label, required, error, type, value, autoComplete, classes } = this.props;

    return (
      <FormControl required={required} className={classes.formControl} error={error ? true : false} aria-describedby={id + '-text'}>
        <InputLabel htmlFor={id}>{label}</InputLabel>
        <Input id={id} onChange={e => this.handleChange(e)} type={type || 'text'} autoComplete={autoComplete} value={value} onKeyDown={(e) => this.handleKeydown(e)} />
        {error && error[id] ? (<FormHelperText id={id + '-text'}>{error[id]}</FormHelperText>) : undefined}
      </FormControl>
    );
  }
}

const styles = theme => ({
  formControl: {
    marginLeft: theme.spacing.unit,
    marginRight: theme.spacing.unit,
    width: 400,
  },
});

export default withStyles(styles)(NumberControl);
