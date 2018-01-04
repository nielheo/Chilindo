import * as React from 'react'

import FormInput from './FormInput'

export default class FormTextbox extends React.Component<{ onChange: any, label: string, error: string, disabled: boolean, value: string }, any> {
  public render() {
    //console.log(this.props.airlines)
    return <FormInput label={this.props.label} error={this.props.error} >
      <input type='textbox'
        className="form-control"
        onChange={this.props.onChange}
        disabled={this.props.disabled}
        value={this.props.value}
      />
    </FormInput>
  }
}