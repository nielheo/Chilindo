import * as React from 'react'

import { Grid, Row, Col, Button, Table, Panel } from 'react-bootstrap'

export default class Batch extends React.Component<{ result: any }, any> {
  public render() {
    console.log(this.props.result)
    let { result } = this.props
    return <section>
      <b>{'Batch ' + result.index} - started: {result.started.format('HH:mm:ss.ms')}</b>
      <hr />
      {
        this.props.result.balance &&
        <Panel header={'Initial Balance'}>
          <Table responsive>
            <thead>
              <tr>
                <th>Curr</th>
                <th>Balance</th>
              </tr>
            </thead>
            <tbody>
              {this.props.result.balance.accountBalances.map((bal: any) =>
                <tr>
                  <td>{bal.currency}</td>
                  <td>{bal.balance.toLocaleString('en-US')}</td>

                </tr>
              )}
            </tbody>
          </Table>
        </Panel>
      }
    </section>
  }
}