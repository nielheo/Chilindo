import * as React from 'react'

import { Grid, Row, Col, Button, Table, Panel } from 'react-bootstrap'

export default class Transaction extends React.Component<{ transactions: any }, any> {
  public render() {
    console.log( this.props.transactions)
    return <Panel>
      <Row><Col md={12}>
      <Table responsive>
        <thead>
          <tr>
            <th>Batch</th>
            <th>Index</th>
            <th>Started</th>
            <th>action</th>
            <th>Amount</th>
            <th>Result</th>
            <th>Ended</th>
              <th>-</th>
          </tr>
        </thead>
        <tbody>
          {this.props.transactions.map((tran: any) =>
            <tr>
              <td>{tran.batch}</td>
              <td>{tran.index}</td>
              <td>{tran.started.format('HH:mm:ss.ms')}</td>
              <td>{tran.action}</td>
              <td>{tran.currency} {tran.amount.toLocaleString('en-US')}</td>
              <td className={tran.result && !tran.result.successful ? 'danger' : ''}>
                {tran.result ? (tran.result.successful
                  ? 'success - ' + tran.result.currency + ' ' + tran.result.balance
                  : 'failed - ' + tran.result.message + ' ' + tran.result.currency + ' ' + tran.result.balance) : '-'}
              </td>
              <td>
                {tran.ended && tran.ended.format('HH:mm:ss.ms') }
              </td>
                <td></td>
            </tr>
          )}
        </tbody>
      </Table>
        </Col></Row>
    </Panel>
  }
}