#!/bin/sh
#

# Must match the RQ-queue name in the flask program
queue_name="RunCode"

exec rq worker --with-scheduler "$queue_name"
