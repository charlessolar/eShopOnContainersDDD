import * as React from 'react';
import { observable, action, computed } from 'mobx';
import { observer } from 'mobx-react';

import { withStyles, WithStyles } from '@material-ui/core/styles';
import Input from '@material-ui/core/Input';
import InputLabel from '@material-ui/core/InputLabel';
import FormControl from '@material-ui/core/FormControl';
import FormHelperText from '@material-ui/core/FormHelperText';

interface TextProps {
  id: string;
  required?: boolean;
  label: string;
  error?: any;
  type?: string;
  value?: any;
  autoComplete?: string;
  fieldProps?: any;
  disabled?: boolean;

  onChange?: (newVal: string) => void;
  onKeyDown?: (key: number, value: string) => void;
}

const styles = theme => ({
  formControl: {
    marginLeft: theme.spacing.unit,
    marginRight: theme.spacing.unit,
    maxWidth: 400,
  },
});

class TextControl extends React.Component<TextProps & WithStyles<'formControl'>, {}> {

  private handleInput = (e: React.ChangeEvent<HTMLTextAreaElement | HTMLInputElement>) => {
    if (this.props.onChange) {
      this.props.onChange(e.target.value);
    }
    e.stopPropagation();
  }
  private handleKeydown = (e: React.KeyboardEvent<HTMLTextAreaElement | HTMLInputElement>) => {
    if (this.props.onKeyDown) {
      this.props.onKeyDown(e.which, (e.target as any).value);
    }
  }

  public render() {
    const { id, label, required, error, type, value, autoComplete, classes, disabled, fieldProps } = this.props;

    return (
      <FormControl required={required} className={classes.formControl} disabled={disabled} fullWidth error={error && error[id] ? true : false} aria-describedby={id + '-text'}>
        <InputLabel htmlFor={id}>{label}</InputLabel>
        <Input id={id} onInput={this.handleInput} type={type || 'text'} autoComplete={autoComplete} value={value || ''} onKeyDown={this.handleKeydown} {...fieldProps} />
        {error && error[id] ? error[id].map((e, key) => (<FormHelperText key={key} id={id + '-' + key + '-text'}>{e}</FormHelperText>)) : undefined}
      </FormControl>
    );
  }
}
export default withStyles(styles)<TextProps>(TextControl);
