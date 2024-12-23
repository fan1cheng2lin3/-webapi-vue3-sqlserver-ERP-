<script setup>
import { ref, reactive, onMounted, watch } from 'vue';
import axios from 'axios';
import { ElMessage } from 'element-plus';

const purchaseForm = reactive({
  orderId: '',
  selectedOrderId: '',
  purchaseDate: '',
  warehouse: '',
  staff: '',
});

const productTableData = ref([]);
const orderList = ref([]);
const StaffList = ref([]);
// 表单引用
const purchaseFormRef = ref(null);
// 提交表单
const submitForm = async () => {
  purchaseFormRef.value.validate(async () => {
    // 构建 payload
    const payload = {
      Name: purchaseForm.warehouse, // 仓库名称
      Storehouses: productTableData.value.map(item => ({
        StorehouseAddress: item.name, // 使用产品名称作为仓库地址
        ProductCode: item.product_code,
        ProductType: item.product_type,
        UnitPrice: item.unit_price,
        Count: item.count
      }))
    };

    try {
      await axios.post('/File_Management/appStorehouse_Table', payload);
      ElMessage.success('添加成功');
    } catch (error) {
      const message = error.response?.data?.message || '网络或服务端错误';
      ElMessage.error(`添加失败：${message}`);
    }
  });
};

const fetchOrderProducts = async () => {
  try {
    const response = await axios.get(`/File_Management/order_products`, {
      params: { order_id: purchaseForm.selectedOrderId },
    });
    const filteredData = response.data.filter(item => item.order_id === purchaseForm.selectedOrderId);
    productTableData.value = filteredData;
  } catch (error) {
    console.error("查询订单商品信息失败", error);
  }
};

const fetchOrders = async () => {
  try {
    const response = await axios.get('/File_Management/purchase_orders');
    orderList.value = response.data;
    if (orderList.value.length > 0) {
      purchaseForm.selectedOrderId = orderList.value[0].order_id;
      fetchOrderProducts();
    }
  } catch (error) {
    console.error("查询订单信息失败", error);
  }
};

const fetchStaffList = async () => {
  // 查询操作人员列表
  // 这里需要根据实际情况编写查询操作人员列表的逻辑
};

// 监听 selectedOrderId 的变化，并重新加载商品信息
watch(() => purchaseForm.selectedOrderId, () => {
  fetchOrderProducts();
});

// 自动生成订单编号
const generateOrderId = () => {
  const now = new Date();
  return (
    now.getMinutes().toString().padStart(2, '0') +
    now.getSeconds().toString().padStart(2, '0') +
    now.getMilliseconds().toString().padStart(3, '0')
  );
};

onMounted(() => {
  fetchOrders();
  fetchStaffList();
  purchaseForm.orderId = generateOrderId();
});
</script>

<template>
  <h1>入库表</h1>
  <el-form-item>
    <el-button type="primary" @click="submitForm">提交</el-button>
  </el-form-item>
  <el-form ref="purchaseFormRef" :model="purchaseForm" :rules="rules" label-width="120px">
    <el-form-item label="入库编号">
      <el-input v-model="purchaseForm.orderId" disabled></el-input>
    </el-form-item>
    <el-form-item label="订单编号">
      <el-select v-model="purchaseForm.selectedOrderId">
        <el-option v-for="order in orderList" :key="order.order_id" :label="order.order_id" :value="order.order_id" />
      </el-select>
    </el-form-item>
    <el-form-item label="到货日期" prop="purchaseDate">
      <el-date-picker v-model="purchaseForm.purchaseDate" type="date" required></el-date-picker>
    </el-form-item>
    <el-form-item label="仓库" prop="warehouse">
      <el-input v-model="purchaseForm.warehouse" required></el-input>
    </el-form-item>
    <el-form-item label="操作人员" prop="staff">
      <el-select v-model="purchaseForm.staff" placeholder="请选择工作人员" required>
        <el-option v-for="staff in StaffList" :key="staff.id" :label="staff.name" :value="staff.id" />
      </el-select>
    </el-form-item>

    <el-table :data="productTableData" style="width: 100%" border>
      <el-table-column label="产品编码" prop="product_code">
        <template #default="scope">
          <el-input v-model="scope.row.product_code" readonly></el-input>
        </template>
      </el-table-column>
      <el-table-column label="产品名称" prop="name">
        <template #default="scope">
          <el-input v-model="scope.row.name" readonly></el-input>
        </template>
      </el-table-column>
      <el-table-column label="产品类型" prop="product_type">
        <template #default="scope">
          <el-input v-model="scope.row.product_type" readonly></el-input>
        </template>
      </el-table-column>
      <el-table-column label="单价" prop="unit_price">
        <template #default="scope">
          <el-input-number v-model="scope.row.unit_price" :min="0" disabled></el-input-number>
        </template>
      </el-table-column>
      <el-table-column label="数量" prop="count">
        <template #default="scope">
          <el-input-number v-model="scope.row.count" :min="0" disabled></el-input-number>
        </template>
      </el-table-column>
    </el-table>
  </el-form>
</template>
