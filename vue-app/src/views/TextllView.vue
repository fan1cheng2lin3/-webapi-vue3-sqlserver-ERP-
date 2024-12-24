
<script setup>
import { ref, onMounted, computed } from 'vue';
import axios from 'axios';
// import { ElTable, ElTableColumn, ElCard } from 'element-plus';

// 存储数据
const apDetailData = ref([]);
const arDetailData = ref([]);

// 获取应付明细数据
const fetchAPDetail = async () => {
  try {
    const response = await axios.get('/Finance/GetAP_Detail');
    apDetailData.value = response.data;
  } catch (error) {
    console.error('获取应付明细数据失败', error);
  }
};

// 获取应收明细数据
const fetchARDetail = async () => {
  try {
    const response = await axios.get('/Finance/GetAR_Detail');
    arDetailData.value = response.data;
  } catch (error) {
    console.error('获取应收明细数据失败', error);
  }
};

// 计算应付总账
const totalAPAmount = computed(() => {
  return apDetailData.value.reduce((sum, item) => sum + item.amount, 0).toFixed(2);
});

// 计算应收总账
const totalARAmount = computed(() => {
  return arDetailData.value.reduce((sum, item) => sum + item.amount, 0).toFixed(2);
});

// 在组件加载时获取数据
onMounted(() => {
  fetchAPDetail();
  fetchARDetail();
});
</script>

<template>
  <div>
    <el-card>
      <h2>应付总账：{{ totalAPAmount }} 元</h2>
    </el-card>

    <el-card>
      <h2>应收总账：{{ totalARAmount }} 元</h2>
    </el-card>

    <h2>应付明细</h2>
    <el-table :data="apDetailData" style="width: 100%" border>
      <el-table-column label="详情ID" prop="detail_id" width="180"></el-table-column>
      <el-table-column label="供应商" prop="receivables" width="180"></el-table-column>
      <el-table-column label="发票号码" prop="invoice_number" width="180"></el-table-column>
      <el-table-column label="金额" prop="amount" width="180">
        <template #default="scope">
          <span>{{ scope.row.amount.toFixed(2) }}</span>
        </template>
      </el-table-column>
      <el-table-column label="到期日期" prop="due_date" width="180">
        <template #default="scope">
          <span>{{ scope.row.due_date ? new Date(scope.row.due_date).toLocaleDateString() : '无' }}</span>
        </template>
      </el-table-column>
      <el-table-column label="创建时间" prop="created_at" width="180">
        <template #default="scope">
          <span>{{ new Date(scope.row.created_at).toLocaleString() }}</span>
        </template>
      </el-table-column>
    </el-table>

    <h2>应收明细</h2>
    <el-table :data="arDetailData" style="width: 100%" border>
      <el-table-column label="详情ID" prop="detail_id" width="180"></el-table-column>
      <el-table-column label="用户" prop="receivables" width="180"></el-table-column>
      <el-table-column label="发票号码" prop="invoice_number" width="180"></el-table-column>
      <el-table-column label="金额" prop="amount" width="180">
        <template #default="scope">
          <span>{{ scope.row.amount.toFixed(2) }}</span>
        </template>
      </el-table-column>
      <el-table-column label="到期日期" prop="due_date" width="180">
        <template #default="scope">
          <span>{{ scope.row.due_date ? new Date(scope.row.due_date).toLocaleDateString() : '无' }}</span>
        </template>
      </el-table-column>
      <el-table-column label="创建时间" prop="created_at" width="180">
        <template #default="scope">
          <span>{{ new Date(scope.row.created_at).toLocaleString() }}</span>
        </template>
      </el-table-column>
    </el-table>
  </div>
</template>

<style scoped>
h2 {
  margin-top: 30px;
  margin-bottom: 20px;
  font-size: 20px;
  font-weight: bold;
  color: #333;
}

.el-table {
  margin-top: 10px;
  margin-bottom: 30px;
}

.el-table-column {
  text-align: center;
}

.el-card {
  margin-bottom: 20px;
}
</style>
